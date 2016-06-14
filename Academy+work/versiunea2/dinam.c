#include <stdlib.h>
#include "dinam.h"

t_queue		*ft_push(t_queue *last, char value)
{
	t_queue *new;
	
	new = (t_queue*)malloc(sizeof(t_queue));
	new->value = value;
	new->next = NULL;
	if (last)
		last->next = new;
	return (new);
}

char		ft_pop(t_queue **last)
{
	char ret;
	t_queue *next;

	ret = (*last)->value;
	next = (*last)->next;
	free(*last);
	(*last) = next;
	return (ret);
}
