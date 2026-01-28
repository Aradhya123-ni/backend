package handlers

import (
	"fmt"
	"log"

	"unityservertictactoe/models"
	"unityservertictactoe/store"
)

func AddToQueue(c *models.Client) {
	store.Mu.Lock()
	defer store.Mu.Unlock()

	if c.State != models.IDLE {
		return
	}

	c.State = models.IN_QUEUE
	store.Queue = append(store.Queue, c)

	if len(store.Queue) >= 2 {
		p1 := store.Queue[0]
		p2 := store.Queue[1]
		store.Queue = store.Queue[2:]
		CreateRoom(p1, p2)
	}
}

func CreateRoom(p1, p2 *models.Client) {
	roomID := fmt.Sprintf("room-%d", store.RoomCounter)
	store.RoomCounter++

	room := &models.Room{
		ID:      roomID,
		PlayerX: p1,
		PlayerO: p2,
		Turn:    "X",
	}

	p1.State, p2.State = models.IN_GAME, models.IN_GAME
	p1.RoomID, p2.RoomID = roomID, roomID
	p1.Symbol, p2.Symbol = "X", "O"

	store.Rooms[roomID] = room

	Send(p1, "match_found", map[string]string{"symbol": "X"})
	Send(p2, "match_found", map[string]string{"symbol": "O"})

	log.Println("Room created:", roomID)
}
