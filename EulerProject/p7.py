import math

def is_prime(nbr):
	if (nbr == 2):
		return (1);
	for d in range(2, int(math.sqrt(nbr)) + 1):
		if (nbr % d == 0):
			return (0);
	return (1);

c = 0;
d = 2;
while (c < 10001):
	if (is_prime(d)):
		c += 1;
	d += 1;
print(d - 1);
	
