#include "csv.h"

static	char	get_next_char(int fd)
{
	char	buffer;
	
	if (read(fd, &buffer, 1))
		return (buffer);
	return (0);
}

static char		is_not_separator(char c)
{
	return ((c != VALUE_SEPARATOR) && (c != RECORD_SEPARATOR));
}

unsigned int	get_next_value(int fd, char **value)
{
	char			actual_char;
	char			value_buffer[BUFF_SIZE];
	unsigned int	index;

	actual_char = get_next_char(fd);
	if (actual_char == 0)
		return (END_OF_FILE);
	index = 0;
	while (is_not_separator(actual_char))
	{
		value_buffer[index] = actual_char;
		actual_char = get_next_char(fd);
		index++;
	}
	(*value) = (char*)malloc(sizeof(char) * index + 1);
	strcpy(*value, value_buffer);
	(*value)[index] = 0;
	if (actual_char == RECORD_SEPARATOR)
		return (END_OF_LINE);
	return (READ_OK);
}
