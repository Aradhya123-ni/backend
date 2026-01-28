package models

import "github.com/gorilla/websocket"

type PlayerState int

const (
	IDLE PlayerState = iota
	IN_QUEUE
	IN_GAME
)

type Client struct {
	ID     string
	Conn   *websocket.Conn
	State  PlayerState
	RoomID string
	Symbol string
}
