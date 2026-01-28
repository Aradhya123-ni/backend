package handlers

import (
	"encoding/json"

	"unityservertictactoe/models"
)

func HandleClient(c *models.Client) {
	defer CleanupClient(c)

	for {
		_, data, err := c.Conn.ReadMessage()
		if err != nil {
			return
		}

		var msg models.Message
		if err := json.Unmarshal(data, &msg); err != nil {
			continue
		}

		switch msg.Type {
		case "find_match":
			AddToQueue(c)
		case "move":
			HandleMove(c, msg.Data)
		case "replay_request":
			HandleReplayRequest(c)
		case "replay_accepted":
			HandleReplayAccepted(c)
		case "replay_denied":
			HandleReplayDenied(c)
		case "quit_game":
			handleQuitGame(c)
			return
		}
	}
}
