package models

type Room struct {
	ID              string
	PlayerX         *Client
	PlayerO         *Client
	Board           [9]string
	Turn            string
	Over            bool
	Replay_requestX bool
	Replay_requestO bool
}
