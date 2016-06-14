#include <string.h>
void	*ft_memcpy(void *dest, const void *src, size_t n)
{
	unsigned char *ddest;
	unsigned char *ssrc;

	ddest = dest;
	ssrc = src;
	while (n--)
		ddest[n] = ssrc[n];
	return (dest);
}
