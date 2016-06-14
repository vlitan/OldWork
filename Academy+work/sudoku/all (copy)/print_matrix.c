#include <unistd.h>

void	ft_putchar(char c)
{
	write(1, &c, 1);
}

void	print_matrix(int matrix[9][9])
{
	int i;
	int j;
	
	i = 0;
	while (i < 9)
	{
		j = 0;
		while (j < 9)
		{
			ft_putchar('0' + matrix[i][j]);
			j++;
		}
		ft_putchar('\n');
		i++;
	}
}
