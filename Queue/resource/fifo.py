from collections import deque

dq = deque(['b','c','d'])
print(dq)

# add to queue
dq.append('e')

print(dq)

# finish queue
dq.popleft()

print(dq)



# print dq

# # adding an element to the right of the queue
# dq.append('e')
# print dq

# # adding an element to the left of the queue
# dq.appendleft('a')
# print dq

# # iterate over deque's elements
# for elt in dq:
#     print(elt)

# # pop out an element at from the right of the queue
# dq.pop()
# print dq

# # pop out an element at from the right of the queue
# dq.popleft()
# print dq

# # print as list
# print list(dq)

# # reversed list
# print list(reversed(dq))

# # empty the deque element
# dq.clear()