#include <stdio.h>
#include "csv.h"

void	print_person(person_t person)
{
	
	printf("name:%s\n", person.name);
	printf("surename:%s\n", person.surename);
	printf("email:%s\n", person.email);
	printf("mark:%f\n", person.mark);
	printf("county:%s\n", person.county);
	printf("--------\n");
}
