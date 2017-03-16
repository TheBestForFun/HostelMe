package utils

import (
	"fmt"
)

// Error print error string str in console
func Error(str string) {
	fmt.Printf("[E] %s\n", str)
}

// Info print message string str in console
func Info(str string) {
	fmt.Printf("[I] %s\n", str)
}
