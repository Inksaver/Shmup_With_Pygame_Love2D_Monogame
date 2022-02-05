import pygame, random, math
import shared, circle

class Mob():
	def __init__(self, images):
		self.colour = shared.RED
		# change variable to allow copy, and choice from list 
		self.image_original = random.choice(images) 
		self.image_original.set_colorkey(shared.BLACK)
		self.image = self.image_original.copy()
		self.rect = self.image.get_rect()
		# using circle collision
		self.radius = int(min(self.rect.width, self.rect.height) * 0.4)
		self.circle = circle.Circle(self.rect.centerx, self.rect.centery, self.radius)
		self.rotation = 0
		self.rotation_speed = 0
		self.time_passed = 0

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
		self.rotation_speed = random.randrange(-10, 10)
		self.circle.x = self.rect.centerx
		self.circle.y = self.rect.centery

	def get_rect(self):
		return self.rect

	def get_circle(self):
		self.circle.x = self.rect.centerx
		self.circle.y = self.rect.centery
		return self.circle	

	def rotate(self, dt):
		self.time_passed += dt
		if self.time_passed >= 0.05:
			self.time_passed = 0		
			self.rotation = (self.rotation + self.rotation_speed) % 360 #rotation autoresets to 0
			new_image = pygame.transform.rotate(self.image_original, self.rotation)
			old_centre = self.rect.center
			self.image = new_image
			self.rect = self.image.get_rect()
			self.rect.center = old_centre
			self.circle.center = self.rect.center
   	
	def get_rotated_rect(self):
		r = math.sqrt(self.rect.width * self.rect.width / 4 + self.rect.height * self.rect.height / 4)
		
		thetas = [math.degrees(math.atan((self.rect.height / 2) / (self.rect.width / 2)))]
		thetas.extend([-thetas[0] - self.rotation,
					   thetas[0] - 180 - self.rotation,
					   180 - thetas[0] - self.rotation])

		thetas[0] -= self.rotation
		vertices = []
		for theta in thetas:
			vertices.append(pygame.math.Vector2(math.cos(math.radians(theta)) * r + self.rect.centerx,
												math.sin(math.radians(theta)) * r + self.rect.centery))

		return vertices	

	def update(self, dt:float) -> None:
		self.rotate(dt)
		self.rect.y	+= round(self.speedy * dt)
		self.rect.x += round(self.speedx * dt)
		if	self.rect.top > shared.HEIGHT + 10 or self.rect.left < -25 or self.rect.right > shared.WIDTH + 20:
			self.reset()

	def draw(self) -> None:
		shared.screen.blit(self.image, self.rect) # draw the image
		if shared.debug:
			pygame.draw.circle(shared.screen, self.colour, self.circle.center, self.circle.radius, 1)
			pygame.draw.rect(shared.screen, shared.BLUE, self.rect, 1)
			pygame.draw.polygon(shared.screen, shared.GREEN, self.get_rotated_rect(), 1)