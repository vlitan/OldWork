/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   read2.c                                            :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: msinca <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/23 20:53:37 by msinca            #+#    #+#             */
/*   Updated: 2015/09/23 20:54:06 by msinca           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include <unistd.h>
#include <fcntl.h>
#include <stdlib.h>
#include "reads.h"
#include <stdio.h>
#include "debug_prints.h"

t_map	get_next_input(char *argv, char *obst, char *free, char *full)
{
	int fd;

	if (argv == NULL)
		fd = 0;
	else
		fd = open(argv, O_RDONLY);
	return (read_input(fd, obst, free, full));
}

int		get_items(int fd, char *obst, char *free, char *full)
{
	char	*buff;
	int		size;
	int		nbr_size;
	int		buff_s;

	buff_s = 20;
	buff = ft_read_to(fd, '\n', &buff_s);
	*free = buff[buff_s - 4];
	*obst = buff[buff_s - 3];
	*full = buff[buff_s - 2];
	buff[buff_s - 4] = '\0';
	size = ft_atoi(buff, &nbr_size);
	if (!free || !obst || !full || !size || (buff[nbr_size + 3] != '\n'))
		return (-1);
	return (size);
}

int		*get_from_line(char *line, int size, char obst, char freec)
{
	int *answ_buff;
	int i;

	i = 0;
	answ_buff = (int*)malloc(sizeof(int) * (size));
	while (i < size)
	{
		if (is_valid_char(line[i], obst, freec))
		{
			if (line[i] == obst)
				answ_buff[i] = 0;
			else
				answ_buff[i] = 1;
		}
		else
		{
		//	printf("I don`t like zis one %c <<--", line[i]);
			answ_buff[0] = -1;
		}
		i++;
	}
	return (answ_buff);
}

t_map	inspect_nd_prepare(int fd, char *obst, char *free, char *full)
{
	t_map	map;
	char	*line;

	map.m = 0x4000;
	map.n = get_items(fd, obst, free, full);
	if (map.n <= 0)
	{
		map.n = 0;
		return (map);
	}
	map.oriz = (int**)malloc(sizeof(int*) * map.n);
	line = ft_read_to(fd, '\n', &map.m);
	if (map.m <= 0)
	{
		map.n = 0;
		return (map);
	}
	(map.m)--;
	map.oriz[0] = get_from_line(line, map.m, *obst, *free);
	if (map.oriz[0][0] == -1)
	{
		map.n = 0;
		return (map);
	}
	return (map);
}

t_map	read_input(int fd, char *obst, char *free, char *full)
{
	char	*line;
	t_map	map;
	int		i;

	map = inspect_nd_prepare(fd, obst, free, full);
	line = (char*)malloc(map.m + 1);
	i = 1;
	while (i < map.n)
	{
		if (read(fd, line, map.m + 1) == 0)
			map.n = 0;
		map.oriz[i] = get_from_line(line, map.m, *obst, *free);
		if (map.oriz[i][0] == -1)
			map.n = 0;
		i++;
	}
	return (map);
}
