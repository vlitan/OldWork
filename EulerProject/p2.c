#include <stdio.h>

int	get_next(int *l1, int *l2)
{
	int aux;

	aux = *l1 + *l2;
	*l2 = *l1;
	*l1 = aux;
	return (aux);
}

int main(int argc, char **argv)
{
	int		n;
	int		i;
	long	sum;
	int		l1, l2;
	
	l1 = l2 = 1;
	n = atoi(argv[1]);
	for (i = 0, sum = 0; l1 < 4000000; i++)
	{
		get_next(&l1, &l2);
		if (!(l1 % 2))
		{
			sum += l1;
		}
	}
	printf("%li\n", sum);
	return (0);
}
