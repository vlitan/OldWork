#include <stdio.h>
#include "ft_list.h"
void	ft_putlist(t_list *l, int type, int new)
{
	while (l)
	{
		if (type == 1)
			printf("%d ", *((int*)l->data));
		else if (type == 2)
			printf("%s ", ((char*)l->data));
		else if (type == 3)
			printf("%c ", *((char*)l->data));
		l = l->next;
	}
	if (new == 1)
		printf("\n");	
}
