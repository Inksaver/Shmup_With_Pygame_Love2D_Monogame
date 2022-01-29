import pygame
import shared

def init(img, speed):
	global image, rect, speedx		# define variables that can be used in all 
	w, h = img.get_size()
	image = pygame.transform.scale(img, (round(w * 0.5), round(h * 0.5)))
	rect = image.get_rect()
	rect.centerx = shared.WIDTH * 0.5	# set the x position 
	rect.bottom = shared.HEIGHT - 10	# set the y position		
	image.set_colorkey(shared.BLACK)	
	speedx = speed			

def update(keystate, dt):
	''' 
	calculate distance to move the rectangle
	Pygame rect is integer only, so using delta time
	requires rounding of calculations to int values
	'''
	if keystate[pygame.K_LEFT] or keystate[pygame.K_a]:
		rect.x -= round(speedx * dt)
	if keystate[pygame.K_RIGHT] or keystate[pygame.K_d]:
		rect.x += round(speedx * dt)
	''' Check if rectangle is out of bounds '''
	if rect.right > shared.WIDTH:
		rect.right = shared.WIDTH
	if rect.left < 0:
		rect.left = 0
	
def draw():
	shared.screen.blit(image, rect)
	if shared.debug:
		pygame.draw.rect(shared.screen, shared.RED, rect, 1)