package handlers

import (
	"log"

	"unityservertictactoe/models"
	"unityservertictactoe/store"
)

func CleanupClient(c *models.Client) {
	store.Mu.Lock()
	room := store.Rooms[c.RoomID]
	store.Mu.Unlock()

	if room != nil {
		var opponent *models.Client
		if c.Symbol == "X" {
			opponent = room.PlayerO
		} else {
			opponent = room.PlayerX
		}

		if opponent != nil {
			Send(opponent, "opponent_left", map[string]string{
				"reason": "disconnect",
			})
		}

		store.Mu.Lock()
		delete(store.Rooms, room.ID)
		store.Mu.Unlock()
	}

	store.Mu.Lock()
	delete(store.Clients, c.ID)
	store.Mu.Unlock()

	c.Conn.Close()
	log.Println("Disconnected:", c.ID)
}
