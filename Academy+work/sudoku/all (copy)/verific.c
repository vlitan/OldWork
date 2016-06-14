/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   verific.c                                          :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: spelea <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/12 21:48:46 by spelea            #+#    #+#             */
/*   Updated: 2015/09/13 17:48:24 by spelea           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */
#include <stdio.h>
int		sqr(int x[9][9], int i, int j)
{
	int il;
	int jl;
	int ic;
	int jc;

	il = (i / 3) * 3;
	jl = (j / 3) * 3;
	ic = il;
	while (ic < il + 3)
	{
		jc = jl;
		while (jc < jl + 3)
		{
			if (x[ic][jc] == x[i][j] && ic != i && jc != j)
				return (0);
			jc++;
		}
		ic++;
	}
	return (1);
}

int		line(int x[9][9], int i, int j)
{
	int a;

	a = 0;
	while (a < 9)
	{
		if (x[i][a] == x[i][j] && j != a)
			return (0);
		a++;
	}
	return (1);
}

int		colon(int x[9][9], int i, int j)
{
	int a;

	a = 0;
	while (a < 9)
	{
		if (x[a][j] == x[i][j] && a != i)
			return (0);
		a++;
	}
	return (1);
}

int		valid(int x[9][9], int i, int j)
{
	if (line(x, i, j) == 0)
		return (0);
	if (colon(x, i, j) == 0)
		return (0);
	if (sqr(x, i, j) == 0)
		return (0);
	return (1);
}
