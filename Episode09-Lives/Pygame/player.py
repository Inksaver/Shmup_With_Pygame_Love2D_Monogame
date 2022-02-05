import pygame
import shared, circle

def init(img, speed, scl):
	global image, rect, speedx, scale, circle, alive, shield, lives, hidden, hide_timer		# define variables that can be used in all 
	w, h = img.get_size()
	scale = scl
	image = pygame.transform.scale(img, (round(w * scale), round(h * scale)))
	rect = image.get_rect()
	rect.centerx = shared.WIDTH * 0.5	# set the x position 
	rect.bottom = shared.HEIGHT - 10	# set the y position		
	image.set_colorkey(shared.BLACK)	
	speedx = speed
	circle = circle.Circle(rect.centerx, rect.centery, rect.width * scale)
	alive = True
	shield = 100
	lives = 3
	hidden = False
	hide_timer = 0

def update(keystate, dt):
	global hidden, hide_timer
	if hidden:
		hide_timer += dt
		if hide_timer > 2:
			hide_timer = 0
			hidden = False
			rect.centerx = shared.WIDTH / 2
			rect.bottom = shared.HEIGHT - 10	

	if keystate[pygame.K_LEFT] or keystate[pygame.K_a]:
		rect.x -= round(speedx * dt)
	if keystate[pygame.K_RIGHT] or keystate[pygame.K_d]:
		rect.x += round(speedx * dt)
	circle.x = rect.center[0]
	''' Check if rectangle is out of bounds '''
	if rect.right > shared.WIDTH:
		rect.right = shared.WIDTH
	if rect.left < 0:
		rect.left = 0	

def draw():
	shared.screen.blit(image, rect)
	if shared.debug:
		pygame.draw.circle(shared.screen, shared.RED, circle.center, circle.radius, 1)
		pygame.draw.rect(shared.screen, shared.BLUE, rect, 1)		

def hide():
	# hide the sprite by moving it off-screen
	global hidden
	hidden = True
	hide_timer = pygame.time.get_ticks()
	rect.center = (shared.WIDTH / 2, shared.HEIGHT + 200)	