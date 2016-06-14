/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   ft_list_clear.c                                    :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: vlitan <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/16 18:29:11 by vlitan            #+#    #+#             */
/*   Updated: 2015/09/16 18:58:51 by vlitan           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "ft_list.h"

void	ft_list_clear(t_list **begin_list)
{
	t_list *next;

	while (*begin_list)
	{
		next = *begin_list;
		free(*begin_list);
		*begin_list = next->next;
	}
	*begin_list = NULL;
}
