package handlers

import (
	"unityservertictactoe/models"
	"unityservertictactoe/store"
)

func HandleReplayRequest(c *models.Client) {
	store.Mu.Lock()
	defer store.Mu.Unlock()

	room := store.Rooms[c.RoomID]
	if room == nil || !room.Over {
		return
	}

	if c.Symbol == "X" {
		room.Replay_requestX = true
		Send(room.PlayerO, "replay_invite", nil)
	} else {
		room.Replay_requestO = true
		Send(room.PlayerX, "replay_invite", nil)
	}
}

func HandleReplayAccepted(c *models.Client) {
	store.Mu.Lock()
	defer store.Mu.Unlock()

	room := store.Rooms[c.RoomID]
	if room == nil || !room.Over {
		return
	}

	if c.Symbol == "X" {
		room.Replay_requestX = true
	} else {
		room.Replay_requestO = true
	}

	if room.Replay_requestX && room.Replay_requestO {
		room.Board = [9]string{}
		room.Turn = "X"
		room.Over = false
		room.Replay_requestX = false
		room.Replay_requestO = false

		Send(room.PlayerX, "replay_start", room.Board)
		Send(room.PlayerO, "replay_start", room.Board)
	}
}

func HandleReplayDenied(c *models.Client) {
	store.Mu.Lock()
	defer store.Mu.Unlock()

	room := store.Rooms[c.RoomID]
	if room == nil {
		return
	}

	room.Replay_requestX = false
	room.Replay_requestO = false

	if c.Symbol == "X" {
		Send(room.PlayerO, "replay_denied", nil)
	} else {
		Send(room.PlayerX, "replay_denied", nil)
	}
}
func handleQuitGame(c *models.Client) {
	store.Mu.Lock()
	defer store.Mu.Unlock()

	room := store.Rooms[c.RoomID]
	if room == nil {
		return
	}

	if room.Over {
		return
	}

	var opponent *models.Client
	if c.Symbol == "X" {
		opponent = room.PlayerO
	} else {
		opponent = room.PlayerX
	}

	if opponent != nil {
		Send(opponent, "opponent_left", map[string]string{
			"reason": "quit",
		})
	}

	delete(store.Rooms, room.ID)
}
