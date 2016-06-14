#include "ft_list.h"
t_list *ft_list_at(t_list *begin_list, unsigned int nbr)
{
	unsigned int i;

	i = 1;
	while (i <= nbr)
	{
		if (!(begin_list))
			return (NULL);
		i++;
		begin_list =  begin_list->next;
	}
	return (begin_list);

