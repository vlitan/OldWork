#ifndef DINAM_H
# define DINAM_H
typedef	struct		s_queue
{
	char			value;
	struct	s_queue	*next;
}					t_queue;
t_queue				*push(t_queue *q, char value);
char				pop(t_queue **q);
#endif
