/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   print.c                                            :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: msinca <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/23 16:40:22 by msinca            #+#    #+#             */
/*   Updated: 2015/09/23 21:03:38 by msinca           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "BSQ.h"
#include <unistd.h>
#include <stdlib.h>

char	get_char(int in, t_chars chrs)
{
	if (in == 0)
		return (chrs.obst);
	if (in > 0)
		return (chrs.free);
	if (in == -1)
		return (chrs.full);
	return (0);
}

int		between(int s, int m, int d)
{
	return ((s <= m) && (m < d));
}

int		on_square(t_square s, int i, int j)
{
	if (between(s.corner.x, i, s.corner.x + s.size))
	{
		if (between(s.corner.y, j, s.corner.y + s.size))
			return (1);
	}
	return (0);
}

void	print_map(t_map map, t_square square, t_chars chrs)
{
	int		i;
	int		j;
	char	p;

	i = 0;
	while (i < map.n)
	{
		j = 0;
		while (j < map.m)
		{
			if (on_square(square, i, j))
				p = chrs.full;
			else
				p = get_char(map.oriz[i][j], chrs);
			write(1, &p, 1);
			j++;
		}
		free(map.oriz[i]);
		write(1, "\n", 1);
		i++;
	}
	free(map.oriz);
}
