/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   back1.c                                            :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: vlitan <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2015/09/12 17:13:52 by vlitan            #+#    #+#             */
/*   Updated: 2015/09/12 23:36:01 by spelea           ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */
#include <unistd.h>
#include <stdio.h>
int	result[9][9];
int	found_results;

int	valid(int x[9][9], int i, int j);
int	parse_input(int output[9][9], char **input);
void	print_error(int code, int *found_errors);

void	print_matrix(int matrix[9][9]);

void	save_matrix(int matrix[9][9])
{
	int i;
	int j;

	if (found_results == 1)
	{
		i = 0;
		while (i < 9)
		{
			j = 0;
			while (j < 9)
			{
				result[i][j] = matrix[i][j];
				j++;
			}
			i++;
		}
	}
}

void	back(int matrix[9][9], int ic, int jc)
{
	int i;
	//int c;
	
	//printf("\n%d, %d", ic, jc);
	//print_matrix(matrix);
	//scanf("%d", &c);
	if (ic == 9)
	{
		found_results++;
		if (found_results == 1)
			save_matrix(matrix);
	}
	else if (found_results != 2)
	{
		if (matrix[ic][jc] == 0)
		{
			i = 1;
			while (i <= 9)
			{
				matrix[ic][jc] = i;
				if (valid(matrix, ic, jc) == 1)
					back(matrix, ic + jc / 8, (jc + 1) % 9);
				i++;
			}
			matrix[ic][jc] = 0;
		}
		else
			back(matrix, ic + jc / 8, (jc + 1) % 9);
	}
}


int	main(int argc, char **argv)
{
	int	matrix[9][9];
	int	valid_matrix;
	int	found_errors;

	found_errors = 0;
	if (argc == 10)
	{
		valid_matrix = parse_input(matrix, argv);
		if (valid_matrix == 1)
			back(matrix, 0, 0);
		else
			print_error(valid_matrix, &found_errors);
		if (found_results == 1)
			print_matrix(result);
		else
			print_error(-3, &found_errors);
	}
	else
	{
		print_error(-4, &found_errors);
	}
	return (0);
}
