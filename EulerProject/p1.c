#include <stdio.h>

int main(void)
{
	int i;
	int sum;

	for (i = 1, sum = 0; i < 1000; i++)
	{
		if (!((i % 5) && (i % 3)))
			sum += i;
	}
	printf("%d\n", sum);
	return (0);
}
