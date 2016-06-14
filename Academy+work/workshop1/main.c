#include <stdio.h>
#include "csv.h"



int main(void)
{
	int fd;
	char *buff;
	person_t	*actual_person;
	
	fd = open("values.csv", O_RDONLY);
	while (get_next_person(fd, &actual_person) != END_OF_FILE)
	{
		if (person_match(*actual_person))
			print_person(*actual_person);
	}
	return (0);
}
