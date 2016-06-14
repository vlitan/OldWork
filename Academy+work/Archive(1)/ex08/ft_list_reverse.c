#include "ft_list.h"

void	ft_list_reverse(t_list **begin_list)
{
	t_list *last;
	t_list *next;

	if (!(*begin_list))
		return ;
	last = NULL;
	while (*begin_list)
	{
		next = (*begin_list)->next;
		(*begin_list)->next = last;
		last = *begin_list;
		*begin_list = next;
	}
	*begin_list = last;
}
