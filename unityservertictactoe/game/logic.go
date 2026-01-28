package game

func CheckWin(b [9]string, s string) bool {
	wins := [][]int{
		{0, 1, 2}, {3, 4, 5}, {6, 7, 8},
		{0, 3, 6}, {1, 4, 7}, {2, 5, 8},
		{0, 4, 8}, {2, 4, 6},
	}
	for _, w := range wins {
		if b[w[0]] == s && b[w[1]] == s && b[w[2]] == s {
			return true
		}
	}
	return false
}

func CheckDraw(b [9]string) bool {
	for _, v := range b {
		if v == "" {
			return false
		}
	}
	return true
}
