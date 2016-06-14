/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   ft_list_push_back.c                                :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: vlitan <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/16 14:49:25 by vlitan            #+#    #+#             */
/*   Updated: 2015/09/16 17:30:28 by vlitan           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "ft_list.h"

void	ft_list_push_back(t_list **begin_list, void *data)
{
	t_list *crt;
	t_list *new_elem;

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
