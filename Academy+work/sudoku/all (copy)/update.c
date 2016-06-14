
int	ft_get_int(char c)
{
	if	(c == '.')
	{
		return (0);
	}
	else if (('0' <= c) && (c <= '9'))
		return (c - '0');
	return (-1);
}

int	parse_input(int output[9][9], char **input)
{
	int i;
	int j;
	

	i = 1;
	while (i <= 9)
	{
		j = 0;
		while (input[i][j] != '\0')
		{
			output[i - 1][j] = ft_get_int(input[i][j]);
			if (output[i - 1][j] == -1)
				return (-2);
			if (j > 9)
				return (-1);
			j++;
		}
		i++;
	}
	return (1);
}
