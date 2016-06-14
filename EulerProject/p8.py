from numpy import prod

def	get_next_nbr(f):
	b = f.read(1)
	if (b == ''):
		return (-1)
	nbr = ord(b) - ord('0');
	return (nbr)

fo = open("nbr8.in", "r+")
p = 1
queue_size = 13
queue = []
for i in range(0, queue_size):
	queue.append(get_next_nbr(fo))
	p *= queue[i]
print queue;
m = p;
while (queue[i] != -1):
	if (queue[0] == 0):
		queue.pop(0);
		p = prod(queue);
	else:
		p /= queue[0];
		queue.pop(0);
	queue.append(get_next_nbr(fo));
	if (queue[12] < 0):
		break;
	print (queue);
	p *= queue[12];
	if (p > m):
		m = p;
print m;
	

