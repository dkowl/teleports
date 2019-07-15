#pragma once
#include <WinSock2.h>
#include "Socket.h"

struct sockaddr to_sockaddr(const SocketAddress& socketAddress);
SocketAddress from_sockaddr(const struct sockaddr& sockaddr);
