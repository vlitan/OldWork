/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   main.c                                             :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: msinca <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/23 20:50:15 by msinca            #+#    #+#             */
/*   Updated: 2015/09/23 20:51:23 by msinca           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "debug_prints.h"
#include "BSQ.h"
#include <stdio.h>
#include "reads.h"
#include "dinam.h"
t_map		get_map(void);
void		show(t_square found);
t_square	find_square(t_map map);
void		print_map(t_map map, t_square square, t_chars chrs);

int			main(int argc, char **argv)
{
	t_map	map;
	int		i;
	t_chars	ch;

	(void)argc;
	(void)argv;

	char tab[5] = "abcde";
	t_queue *tail;
	t_queue *head;

	head = tail;
	tail = ft_push(tail, tab[0]);
	tail = ft_push(tail, tab[1]);
	tail = ft_push(tail, tab[2]);
	printf("%c%c%c", ft_pop(&head), ft_pop(&head), ft_pop(&head));
	return (0);
	if (argc == 1)
	{
		map = get_next_input(NULL, &(ch.obst), &(ch.free), &(ch.full));
		if (map.n == 0)
			printf("map error\n");
		else
			print_map(map, find_square(map), ch);
	}
	i = 1;
	while (i < argc)
	{
		map = get_next_input(argv[i], &(ch.obst), &(ch.free), &(ch.full));
		if (map.n == 0)
			printf("map error\n");
		else
			print_map(map, find_square(map), ch);
		i++;
	}
	return (0);
}
