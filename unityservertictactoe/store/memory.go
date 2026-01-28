package store

import (
	"sync"

	"unityservertictactoe/models"
)

var (
	Clients     = make(map[string]*models.Client)
	Queue       = make([]*models.Client, 0)
	Rooms       = make(map[string]*models.Room)
	RoomCounter = 1
	Mu          sync.Mutex
)
