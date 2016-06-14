#include "BSQ.h"

#include <stdio.h>
/*
*returns 1 if a and b were consecutive in tab, if inserted
0 otherwise, or if they are in tab already
*/
int		b_search(int *tab, int size, int a, int b)
{
	int l;
	int r;
	int m;

	l = 0;
	r = size - 1;
	if (tab[r] < a)
		return (1);
	else if (tab[l] > b)
		return (1);
	while (l <= r)
	{
		m = l + (r - l) / 2;
		if (tab[m] > a)
			r = m - 1;
		else if (tab[m] < a)
			l = m + 1;
		if (tab[m] == a)
			return (0);
	}
	if (tab[m + 1] <= b)
		return (0);
	return (1);
}
