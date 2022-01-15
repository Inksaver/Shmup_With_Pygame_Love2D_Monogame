import pygame, random
import shared

class Mob():
	def __init__(self, w:int, h:int, colour:tuple) -> None:
		self.rect = pygame.Rect(0, 0, w, h)		# create a rectangle at 0,0 measuring w x h	
		self.colour = colour					# set colour
		self.reset()

	def reset(self) -> None:
		# allowing for the width of the sprite itself
		self.rect.x = random.randrange(0, shared.WIDTH - self.rect.width)
		# start off the top of the screen by random amount
		self.rect.y = random.randrange(-150, -100)
		# set speed to a random amount, so some will move faster than others
		self.speedy = random.randrange(50, 600)
		self.speedx = random.randrange(-200, 200)		
			
	def update(self, dt:float) -> None:
		self.rect.y	+= round(self.speedy * dt)
		self.rect.x += round(self.speedx * dt)
		if	self.rect.top > shared.HEIGHT + 10 or self.rect.left < -25 or self.rect.right > shared.WIDTH + 20:
			self.reset()
			
	def draw(self) -> None:
		pygame.draw.rect(shared.screen, self.colour, self.rect)