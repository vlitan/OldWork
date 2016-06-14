#include <stdio.h>

int is_pali(int nbr)
{
	int aux;
	int new;
	
	aux = nbr;
	new = 0;
	while (nbr)
	{
		new = new * 10 + nbr % 10;
		nbr /= 10;
	}
	return (new == aux);
}

int main(void)
{
	int i, j;
	
	for (i = 999; i > 100; i--)
		for (j = i; j > 100; j--)
			if (is_pali(i * j))
			{
				printf("%d * %d = %d\n", i, j, i * j);
		//		return (0);
			}
	return (0);
}
