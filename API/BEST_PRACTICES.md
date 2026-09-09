# Integration Best Practices

Answers to common questions about making an integration that Neuro plays well with. The protocol mechanics described here are verified against the current server. Guidance about what she handles well is directional and may shift over time, so treat it as a starting point rather than a hard limit.

## Context

- Write context messages in Markdown (or plaintext). Markdown is preferred, but avoid `#` top-level headings; if you need structure, start at `##`. JSON is not rejected, but readable prose or a bulleted list works better than a raw data dump.
- Prefer occasional meaningful messages over a constant stream. Don't send things like continuous position updates; she does not react well to being spammed with many small messages.
- Long explanations (game rules, controls) are fine to send once at startup and repeat at key moments (new level, first encounter). For information she needs to act on right now, prefer the `state` field of an actions force.
- It is safe to send context while an action result is still pending. The server holds it and delivers it in order once the result arrives. Still, send the result first when you can.

## Actions

- Neuro sees the `name`, `description` and `schema` of registered actions exactly as you send them. Use descriptive names (`use_item`, not `action_3`), because she reads them. She always knows which actions are currently registered, so a "list my actions" action is unnecessary.
- Keep descriptions to a sentence or two. Longer rules belong in context messages.
- Keep your set of registered actions stable. Register actions relevant throughout normal gameplay at startup, even if they are temporarily unusable. Register mode-specific actions when entering that mode and unregister them when leaving it. Avoid rapidly registering and unregistering actions: frequent changes to the action set slow down her responses, which matters a lot if you want her gameplay to be fast.
- Multiple similar actions vs. one parameterized action: both work.
  - Use separate actions for genuinely different verbs (`move`, `attack`, `end_turn`).
  - Use one action with an `enum` parameter for a fixed set of homogeneous choices (doors, colors, difficulty levels). She cannot forget the valid options, since they are part of the schema.
  - For choices whose options change frequently (cards in hand, items in inventory), prefer a stable schema with a free-form or broad parameter over re-registering a new enum every time the options change. Validate the value yourself and return a failing result listing the valid options when she gets it wrong.
  - Avoid registering many near-identical actions; she tends to fixate on a few of them.
- Action names are scoped to the character and shared with any other integration connected at the same time. If your integration is meant to run alongside others, pick names unlikely to collide.
- If you receive an action you don't recognize, reply with a failing result saying so rather than staying silent. Results for unknown or stale ids are discarded by the server, so replying is always safe, while staying silent leaves Neuro waiting.

## Action Results

- Send the result as soon as you have _validated_ the action, before executing it in-game. Every second of delay is a second Neuro spends frozen waiting.
- If no result arrives within a short window (currently around 20 seconds), the server treats the action as failed on its own and discards any late result. Do not rely on this; always answer.
- A `success: false` result during an actions force is automatically retried (a limited number of times). Write the `message` as an actionable error, e.g. `"You can't discard a card that isn't in your hand. Your hand is: ..."`, so the retry can actually succeed.
- Outside of a force there is no automatic retry: she reads the failure message and decides for herself whether to try again.
- On success, use the `message` to describe any state that changed as a result of her action that would be helpful for her to know (e.g. the card she drew, the new board state). If nothing meaningful changed, omit it.

## Forcing Actions

- Use `actions/force` whenever the game is blocked waiting on her decision (her turn, a dialog choice). Without a force she will eventually get distracted and forget about the game. For open-ended situations, a timeout-then-force pattern works well.
- Send one force at a time and wait for its resolution: a new force while another is in progress can cancel and replace it, as the spec warns.
- `priority` guidance: keep the default `low` for turn-based games, since it never interrupts her. Use `medium`/`high` when the game state is time-sensitive and waiting through a long monologue would hurt. Reserve `critical` for hard real-time moments (a quick time event about to expire), since it cuts her off mid-sentence.
- `state` and `query` are passed to her verbatim, so any format works; Markdown is recommended because it reads as prose. Indentation of embedded JSON doesn't matter.
- Set `ephemeral_context: true` when you re-send bulky state every turn, so each dump is only visible for that decision instead of being remembered forever.
- Chained forces (pick card, then pick target, then pick discards) work and she understands them, but each link adds a full response round-trip. If an action only needs two or three parameters, prefer one action with a slightly bigger schema.

## Disconnecting and Reconnecting

- After reconnecting, re-send `startup` and re-register your actions immediately; don't wait to be asked.
- Context survives disconnects: what you told her before is still known to her when you come back.

## Game Design

- How much hidden information to reveal to her is partly a content decision, but don't let her cheat. Giving her information she shouldn't have makes her play feel unauthentic. Prefer giving her the same information a human player would have, and let her earn her wins.
- She copes fine with moderate amounts of information (a list of ~20 items is no problem), but she is much better at reading clearly stated information than at inferring what unstated information means. Be painfully explicit.
- She may still attempt impossible actions or send malformed data. Validate everything and answer with a failing result and a clear reason; never assume the data matches the schema.
