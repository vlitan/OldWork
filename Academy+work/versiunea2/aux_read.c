/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   aux_read.c                                         :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: msinca <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/23 20:55:23 by msinca            #+#    #+#             */
/*   Updated: 2015/09/23 20:55:25 by msinca           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>

char	*ft_read_to(int fd, char end, int *buff_s)
{
	char	*mini_buff;
	char	*buff;
	int		count;
	char	*dest;

	count = 0;
	mini_buff = (char*)malloc(1);
	buff = (char*)malloc(*buff_s);//go away
	while (*mini_buff != end)
	{
		read(fd, mini_buff, 1);
		buff[count] = *mini_buff;//push
		count++;
	}
	*buff_s = count;
	dest = (char*)malloc(count);
	count--;
	while (count >= 0)
	{
		dest[count] = buff[count];//pop
		count--;
	}
	return (dest);
}

void	copy_data(int *to, int *from, int count)
{
	int i;

	i = 0;
	while (i < count)
	{
		to[i] = from[i];
		i++;
	}
}

int		is_valid_char(char in, char obst, char free)
{
	if (in == obst)
		return (1);
	if (in == free)
		return (2);
	return (0);
}

int		ft_atoi(char *str, int *size)
{
	int rez;

	rez = 0;
	*size = 0;
	while (('0' <= str[*size]) && (str[*size] <= '9'))
	{
		rez = rez * 10 + str[*size] - '0';
		(*size)++;
	}
	return (rez);
}
