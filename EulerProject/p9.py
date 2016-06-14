#!/usr/bin/python

def	sq(n):
	return (n * n)
for a in range(1, 10000):
	for b in range(1, 10000):
		if (sq(a) + sq(b) == sq(1000 - a - b)):
			print (a * b * (1000 - a - b))
