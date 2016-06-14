#ifndef CSV_H
# define CSH_H

# include <string.h>
# include <unistd.h>
# include <stdlib.h>
# include <fcntl.h>
# include <stdio.h>

#define BUFF_SIZE			20
#define	READ_OK				1
#define END_OF_LINE			-1
#define	END_OF_FILE			-2
#define	INVALID_NAME		-3
#define	INVALID_SURENAME	-4
#define	INVALID_MARK		-5
#define INVALID_COUNTY		-6
#define INVALID_EMAIL		-7
#define	VALUE_SEPARATOR		';'
#define	RECORD_SEPARATOR	'\n'

typedef	struct		person_s
{
	char			*name;
	char			*surename;
	char			*email;
	char			*county;
	float			mark;
}					person_t;

unsigned int	get_next_value(int fd, char **value);

int				name_valid(char*);
int				surename_valid(char*);
int				county_valid(char*);
int				email_valid(char*);
int				mark_valid(char*);

int				name_match(char*);
int				surename_match(char*);
int				county_match(char*);
int				email_match(char*);
int				mark_match(float);
int				person_match(person_t);

#endif
