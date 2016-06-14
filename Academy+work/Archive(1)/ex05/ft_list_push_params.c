/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   ft_list_push_params.c                              :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: vlitan <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/16 18:13:23 by vlitan            #+#    #+#             */
/*   Updated: 2015/09/16 18:27:29 by vlitan           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "ft_list.h"

void		ft_list_push_back(t_list **begin_list, void *data)
{
	t_list	*crt;
	t_list	*new_elem;

	new_elem = ft_create_elem(data);
	if (*begin_list == NULL)
	{
		*begin_list = new_elem;
		return ;
	}
	crt = *begin_list;
	while (crt->next)
	{
		crt = crt->next;
	}
	crt->next = new_elem;
}

t_list		*ft_list_push_params(int ac, char **av)
{
	t_list *l;

	l = 0;
	while (ac > 1)
	{
		ft_list_push_back(&l, av[ac - 1]);
		ac--;
	}
	return (l);
}
