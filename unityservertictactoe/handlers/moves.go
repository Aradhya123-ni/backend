package handlers

import (
	"encoding/json"

	"unityservertictactoe/game"
	"unityservertictactoe/models"
	"unityservertictactoe/store"
)

type MoveData struct {
	Index int `json:"index"`
}

func HandleMove(c *models.Client, raw json.RawMessage) {
	store.Mu.Lock()
	room := store.Rooms[c.RoomID]
	store.Mu.Unlock()

	if room == nil || room.Over || c.Symbol != room.Turn {
		return
	}

	var move MoveData
	if err := json.Unmarshal(raw, &move); err != nil {
		return
	}

	store.Mu.Lock()
	defer store.Mu.Unlock()

	if move.Index < 0 || move.Index >= 9 || room.Board[move.Index] != "" {
		return
	}

	room.Board[move.Index] = c.Symbol
	if room.Turn == "X" {
		room.Turn = "O"
	} else {
		room.Turn = "X"
	}

	Send(room.PlayerX, "board_update", room.Board)
	Send(room.PlayerO, "board_update", room.Board)

	if game.CheckWin(room.Board, c.Symbol) {
		room.Over = true
		Send(room.PlayerX, "game_over", map[string]string{"winner": c.Symbol})
		Send(room.PlayerO, "game_over", map[string]string{"winner": c.Symbol})
		return
	}

	if game.CheckDraw(room.Board) {
		room.Over = true
		Send(room.PlayerX, "game_over", map[string]string{"winner": "draw"})
		Send(room.PlayerO, "game_over", map[string]string{"winner": "draw"})
	}
}
