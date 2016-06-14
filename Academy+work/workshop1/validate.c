/*
	mock-up validation
*/
#include <string.h>
#include <stdlib.h>

static int only_alpha(char *str)
{
	while (*str)
	{
		if (!isalpha(*str))
			return(0);
	}
	return (1);
}

static char	*new_tolower(char *str)
{
	char *new;

	new = (char*)malloc(strlen(str) * sizeof(char));
	while (*str)
		*(new++) = tolower(*(str++));
	return (new);
}

int			name_valid(char *name)
{
	if (only_alpha(name) && (strlen(name) > 2))
		return (1);
	else
		return (0);
}

//same conditions
int			surename_valid(char *surename)
{
	if (name_valid(surename))
		return (1);
	else
		return (0);
}

int			county_valid(char *county)
{
	char	*lowed;
	
	lowed = new_tolower(county); 
	if (strcmp(lowed, "cluj") &&
		strcmp(lowed, "bihor") &&
		strcmp(lowed, "neamt") &&
		//etc
		strcmp(lowed, "constanta"))
		return (0);
	else
		return (1);
}

int			email_valid(char *email)
{
	int	dot_pos;
	int	at_pos;
	int	str_len;

	dot_pos = (int)(strrchr(email, '.') - email);
	at_pos = (int)(strchr(email, '@') - email);
	str_len = (int)strlen(email);
	if ((at_pos > 1) && (dot_pos < str_len) && (at_pos < dot_pos - 1))
		return (1);
	else
		return (0);
}

int			mark_valid(char *mark)
{
	if (*mark == 0)
		return (0);
	if (*mark == '-')
		mark++;
	if (*mark == '.')
		return (0);
	if (mark[strlen(mark) - 1] == '.')
		return (0);
	while (*mark)
		if (!isdigit(*(mark++)))
			return (0);
	return (1);
}
