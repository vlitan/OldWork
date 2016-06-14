#include "csv.h"

int		get_next_person(int fd, person_t **new_person)
{
	char	*temp;
	int		eof_flag;

	*new_person = (person_t*)malloc(sizeof(person_t));
	//read and validate NAME
	get_next_value(fd, &((**new_person).name));
	if (!name_valid((**new_person).name))
		return (INVALID_NAME);
	printf("mkay");
	return (0);	//read and validate SURENAME
	get_next_value(fd, &((**new_person).surename));
	if (!surename_valid((**new_person).surename))
		return (INVALID_SURENAME);
	//read and validate EMAIL
	get_next_value(fd, &((**new_person).email));
	if (!email_valid((**new_person).email))
		return (INVALID_EMAIL);
	//read and validate MARK
	get_next_value(fd, &temp);
	if (!mark_valid(temp))
		return (INVALID_MARK);
	(**new_person).mark = atof(temp);
	//read and validate COUNTY
	eof_flag = get_next_value(fd, &((**new_person).county));
	if (!county_valid((**new_person).name))
		return (INVALID_COUNTY);
	if (eof_flag == END_OF_FILE)
		return (END_OF_FILE);
	return (1);
}

