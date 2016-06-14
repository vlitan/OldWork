/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   create_matrix.c                                    :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: msinca <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/23 13:21:13 by msinca            #+#    #+#             */
/*   Updated: 2015/09/23 21:01:15 by msinca           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "BSQ.h"
#include "reads.h"
#include "debug_prints.h"
#include "proc.h"
#include <stdio.h>

int			min(int a, int b, int c)
{
	int min;

	min = 0x7fffffff;
	if (a <= b && a <= c)
		min = a;
	else if (b <= a && b <= c)
		min = b;
	else if (c <= a && c <= b)
		min = c;
	return (min);
}

int			find_min(t_map map, int i, int j)
{
	int a;
	int b;
	int c;

	a = map.oriz[i + 1][j + 1];
	b = map.oriz[i][j + 1];
	c = map.oriz[i + 1][j];
	return (min(a, b, c));
}

t_square	line_column(t_map map, int orient)
{
	int			i;
	t_square	answ;

	i = 0;
	answ.corner.x = 0;
	answ.corner.y = 0;
	answ.size = 1;
	if (orient == 1)
		while (i < map.n)
		{
			if (map.oriz[i][0] == 1)
			{
				answ.corner.x = i++;
				return (answ);
			}
		}
	else
		while (i < map.m)
			if (map.oriz[0][i] == 1)
			{
				answ.corner.y = i++;
				return (answ);
			}
	return (answ);
}

t_square	find_square(t_map map)
{
	int			i;
	int			j;
	t_square	max;

	i = map.n - 2;
	if (map.m == 1)
		return (line_column(map, 1));
	else if (map.n == 1)
		return (line_column(map, 2));
	max.size = 0;
	while (i >= 0)
	{
		j = map.m - 2;
		while (j >= 0)
		{
			if (map.oriz[i][j] != 0)
			{
				map.oriz[i][j] = find_min(map, i, j) + 1;
				if (map.oriz[i][j] >= max.size)
				{
					max.size = map.oriz[i][j];
					max.corner.x = i;
					max.corner.y = j;
				}
			}
			j--;
		}
		i--;
	}
	return (max);
}
