package handlers

import (
	"encoding/json"

	"unityservertictactoe/models"

	"github.com/gorilla/websocket"
)

func Send(c *models.Client, t string, d interface{}) {
	msg, _ := json.Marshal(models.Message{
		Type: t,
		Data: mustJSON(d),
	})
	c.Conn.WriteMessage(websocket.TextMessage, msg)
}

func mustJSON(v interface{}) json.RawMessage {
	b, _ := json.Marshal(v)
	return b
}
