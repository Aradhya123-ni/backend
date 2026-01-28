package handlers

import (
	"log"
	"net/http"

	"unityservertictactoe/config"
	"unityservertictactoe/models"
	"unityservertictactoe/store"
)

func WSHandler(w http.ResponseWriter, r *http.Request) {
	conn, err := config.Upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Println("Upgrade error:", err)
		return
	}

	client := &models.Client{
		ID:    conn.RemoteAddr().String(),
		Conn:  conn,
		State: models.IDLE,
	}

	store.Mu.Lock()
	store.Clients[client.ID] = client
	store.Mu.Unlock()

	log.Println("Connected:", client.ID)

	go HandleClient(client)
}
