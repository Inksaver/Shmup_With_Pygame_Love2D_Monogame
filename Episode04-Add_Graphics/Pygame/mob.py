import pygame, random
import shared

class Mob():
	def __init__(self, img):
		self.image = img
		self.image.set_colorkey(shared.BLACK)	 
		self.rect = self.image.get_rect()
		# make the mob appear on screen randomly across it's width
		self.reset()		
	
	def reset(self) -> None:
		# allowing for the width of the sprite itself
		self.rect.x = random.randrange(0, shared.WIDTH - self.rect.width)
		# start off the top of the screen by random amount
		self.rect.y = random.randrange(-150, -100)
		# set speed to a random amount, so some will move faster than others
		self.speedy = random.randrange(50, 400)
		self.speedx = random.randrange(-100, 100)
		if shared.debug:
			self.speedy = random.randrange(10, 100)
			self.speedx = random.randrange(-20, 20)
		
	def get_rect(self):
		return self.rect	
	
	def update(self, dt:float) -> None:
		self.rect.y	+= round(self.speedy * dt)
		self.rect.x += round(self.speedx * dt)
		if	self.rect.top > shared.HEIGHT + 10 or self.rect.left < -25 or self.rect.right > shared.WIDTH + 20:
			self.reset()
			
	def draw(self) -> None:
		shared.screen.blit(self.image, self.rect)
		if shared.debug:
			pygame.draw.rect(shared.screen, shared.RED, self.rect, 1)		