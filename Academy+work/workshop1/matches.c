#include "csv.h"

int		name_match(char *name)
{
	return (1);
}

int		surename_match(char *surename)
{
	return (1);
}

int		county_match(char *county)
{

	if (strcmp(county, "Cluj") == 0)
		return (1);
	if (strcmp(county, "cluj") == 0)
		return (1);
	return (0);
}

int		email_match(char *email)
{
	return (1);
}

int		mark_match(float mark)
{
	if (mark >= 8)
		return (1);
	return (0);
}

int		person_match(person_t person)
{
	
	if (!name_match(person.name))
		return (0);
	if (!surename_match(person.surename))
		return (0);
	if (!email_match(person.email))
		return (0);
	if (!mark_match(person.mark))
		return (0);
	if (!county_match(person.county))
		return (0);
	return (1);
}
