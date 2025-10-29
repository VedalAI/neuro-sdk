import express from "express";
import {json} from "body-parser";
import {WebSocket, WebSocketServer} from "ws";
import {JSONSchemaFaker} from "json-schema-faker";
import util from "util";

const app = express();
app.use(json());
app.listen(1337);

app.post("/", (req, res) => {
    send(req.body);
    res.sendStatus(200);
});

const wss = new WebSocketServer({port: 8000});

let actionId: number = 0;
let connections: WebSocket[] = [];
let actions: Action[] = [];
let pendingResult: { id: string; actionName: string } | null = null;
let actionForceQueue: string[] = [];

wss.on("connection", function connection(ws) {
    console.log("+ Connection opened");
    connections.push(ws);
    ws.on("message", data => {
		try {
			onMessageReceived(JSON.parse(data.toString()) as Message);
		} catch {
			console.error("Caught exception trying to parse message: ");
			console.error(data.toString());
		}
	});
    ws.on("close", () => {
        console.log("- Connection closed");
        connections = connections.filter(c => c != ws);
    });

    send({command: "actions/reregister_all"});
});

function sendAction(actionName: string) {
    const id = actionId.toString();
	actionId++;

    if (actionName == "choose_name") {
        send({command: "action", data: {id, name: "choose_name", data: JSON.stringify({name: "RANDY"})}});
        return;
    }

    const action = actions.find(a => a.name === actionName);
    if (!action) return;

    const responseObj = !action?.schema ? undefined : JSON.stringify(JSONSchemaFaker.generate(action.schema));

    send({command: "action", data: {id, name: action.name, data: responseObj}});
}

async function onMessageReceived(message: Message) {
    console.log("<---", util.inspect(message, false, null, true));

    switch (message.command) {
        case "actions/register": {
            actions.push(...(message.data.actions as Action[]));
            break;
        }

        case "actions/unregister": {
            actions = actions.filter(a => !message.data.action_names.includes(a.name));
            break;
        }

        case "actions/force": {
            const actionName: string = message.data.action_names[Math.floor(Math.random() * message.data.action_names.length)];
            if (pendingResult === null) {
                setTimeout(() => sendAction(actionName), 500);
            } else {
                console.warn("! Received new actions/force while waiting for result; sent to queue");
                actionForceQueue.push(actionName);
            }
            break;
        }

        case "action/result": {
            if (pendingResult === null) {
                console.warn(`! Received unexpected action/result: '${message.data.id}'`);
                break;
            }

            if (message.data.id === pendingResult.id) {
                const actionName = pendingResult.actionName;
                pendingResult = null;

                if (!message.data.success) {
                    setTimeout(() => sendAction(actionName), 500);
                } else if (actionForceQueue.length > 0) {
                    setTimeout(() => sendAction(actionForceQueue.shift()), 500);
                }
            } else {
                console.warn(`! Received unknown action/result '${message.data.id}' while waiting for '${pendingResult.id}'`);
            }
            break;
        }
    }
}

export function send(msg: Message) {
	if (msg.command === "action") {
		pendingResult = {id: msg.data.id, actionName: msg.data.name};
	}
	
    console.log("--->", util.inspect(msg, false, null, true));

    const msgStr = JSON.stringify(msg);
    for (const connection of connections) {
        connection.send(msgStr);
    }
}

type Message = {
    command: string,
    data?: { [key: string]: any }
}

type Action = {
    name: string,
    schema: any
}
