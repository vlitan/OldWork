#include <stdio.h>
#include <math.h>
#define N	(600851475143)

int is_prime(unsigned long i)
{
	unsigned long d;
	unsigned long s;

	s = (int)sqrt(i);
	d = 3;
	while (d <= s)
	{
		if (i % d == 0)
			return (0);
		d += 2;
	}
	return (1);
	
}
int main(void)
{
	unsigned long f;
	f = (unsigned long) sqrt(N);
	while (!((N % f == 0) && (is_prime(f))))
	{
		f--;
	}
	printf("%li\n", f);
	return (0);
}
