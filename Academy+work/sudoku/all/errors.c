#include <unistd.h>

void	ft_putstr(char *c, int output)
{
	int i;
	
	i = 0;
	while (c[i] != '\0')
	{
		write(output, c + i, 1);
		i++;
	}
}

void	print_error(int code, int *found_errors)
{
	
	(void)code;
	if (*found_errors == 0)
	{
		ft_putstr("Erreur\n", 1);
	}
	(void)(*found_errors)++;
/*
	if (code == -1)
		ft_putstr("too much chars in input line", 2);
	else if (code == -2)
		ft_putstr("invalid char in input", 2);
	else if (code == -3)
		ft_putstr("too much or none results", 2);
	else if (code == -4) 
		ft_putstr("wrong number of arguments", 2);*/
}
